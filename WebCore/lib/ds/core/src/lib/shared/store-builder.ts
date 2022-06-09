import { Observable, OperatorFunction, of, ReplaySubject, merge, forkJoin, Subject } from 'rxjs';
import { PASaveHandler, httpError, AttachErrorHandler } from './shared-api-fn';
import { switchMap, first, tap, publishReplay, refCount, withLatestFrom, map, startWith, catchError } from 'rxjs/operators';
import { Maybe } from './Maybe';

type IdOrNewValue<T> = number | string | T;

export interface SetupFnInput<M, U> {
/** A value to identify a unique value in the store or a new item to create */
idOrNewValue: M;
/** An object containing the data we retrieved from the server */
currentValue: U;
}

export interface SetupFnOutput<T, U, M> {
    /** An existing item we want to modify in the store */
  item: T;
  idOrNewValue: M;
  currentValue: U;
}

export function IsSetupFnInput(object): object is SetupFnInput<any,any> {
const myInterface = object as SetupFnInput<any,any>;
return myInterface.idOrNewValue !== undefined && myInterface.currentValue !== undefined;
}

/**
 * See `StoreAction` for a description on these generic types 
 */
export type SetupFn<T, U, M extends IdOrNewValue<T>> = OperatorFunction<SetupFnInput<M,U>,SetupFnOutput<T,U,M>>;

  /**
   * See `StoreAction` for a description on these generic types 
   */
export type ComposableSetupFn<T, U, M extends IdOrNewValue<T>> = (fn?: SetupFn<T, U, M>) => SetupFn<T, U, M>;

export interface StoreActionNoType {
    /**
     * The source of the user's action (like a button click).  When a certain value in the store needs to be found this should
     * pass the unique identifier for that value.
     */
    dispatcher$: Observable<any>;
    /**
     * This is what we want to do as a result of the user's input
     */
    effect: (item: any, idOrNewValue: IdOrNewValue<any>) => Observable<any>;
    /**
     * A function defining how we should update the store value
     */
    updateState: (effectResult: any, oldState: any) => any;
    /**
     * A user friendly string to present to the user when our api call succeeds or fails @see PASaveHandler
     */
    operation: string;
    /**
     * A function that wraps our action with default save result handling @see PASaveHandler
     */
    normalizeSaveHandler: PASaveHandler;
    /**
     * Any other side effects (like opening a modal or finding an existing item in the store) we want to do before
     * calling the provided `effect`.
     */
    setupFn: any;
}

/**
 * An object representing a way that the user can modify data in the store.
 * 
 * @template T An item in the store.
 * @template U The type of the value persisted in the store.
 * @template M The type of the id for items in the store.
 */
export interface StoreAction<T, U, M extends IdOrNewValue<T>> extends StoreActionNoType {
    /**
     * The source of the user's action (like a button click).  When a certain value in the store needs to be found this should
     * pass the unique identifier for that value.
     */
    dispatcher$: Observable<M>;
    /**
     * This is what we want to do as a result of the user's input
     */
    effect: (item: T, idOrNewValue: IdOrNewValue<T>) => Observable<T>;
    /**
     * A function defining how we should update the store value
     */
    updateState: (effectResult: T, oldState: U) => U;
    /**
     * A user friendly string to present to the user when our api call succeeds or fails @see PASaveHandler
     */
    operation: string;
    /**
     * A function that wraps our action with default save result handling @see PASaveHandler
     */
    normalizeSaveHandler: PASaveHandler;
    /**
     * Any other side effects (like opening a modal or finding an existing item in the store) we want to do before calling
     * the provided `effect`.
     */
    setupFn: SetupFn<T, U, M>;
}

/**
 * @class
 * 
 * Creates a stream that handles caching and api calls to modify data on the 
 * server.  Just provide the api call to get the required data from the server and then 
 * configure the actions to modify that data.
 * 
 */
export class StoreBuilder<U> {
    private dataSource: Observable<U>;
    private actions: StoreActionNoType[] = [];

    private readonly ensureAllPropsHaveValue = (value: StoreActionNoType) => {
        const actions = Object.keys(value);
        actions.forEach(key => {
            if (value[key] === undefined || value[key] === null) {
                throw new Error(key + ' must have a value!');
            }
        });
    }

    /**
     * @returns A function that does nothing except map the input into the correct output type.
     */
    readonly nullSetupFn: <T, M extends IdOrNewValue<T>>() => SetupFn<T, U, M> = () => x => x.pipe(map(y => ({
    item: null,
    currentValue: y.currentValue,
    idOrNewValue: y.idOrNewValue
    })))

    /**
     * Creates a configurable object of the correct StoreAction type.
     */
    readonly scaffoldAction: <T, M extends IdOrNewValue<T>>() => StoreAction<T, U, M> = <T, M extends IdOrNewValue<T>>() => {
        return {} as StoreAction<T, U, M>;
    }

    /**
     * Set the api call used to initialize the store.
     * @param source A stream that emits the data to be put into the store.
     * @param attachErrorHandlerFn See `AttachErrorHandler`
     * @param operation A parameter passed to the `AttachErrorHandler` function. See `AttachErrorHandler`
     * @param continueStreamOnFailure A parameter passed to the `AttachErrorHandler` function. See `AttachErrorHandler`
     */
    readonly setDataSource: (
        source: Observable<U>, 
        attachErrorHandlerFn: AttachErrorHandler,
        operation: string,
        continueStreamOnFailure?: boolean) => StoreBuilder<U> = (source, fn, operation, continueStreamOnFailure) => {
        this.dataSource = fn(source, operation, !!continueStreamOnFailure);
        return this;
    }

    /**
     * Adds the provided `StoreAction` to the list of `StoreAction`s  that define how to mutate the value in the store.
     */
    readonly addAction: <T, M extends IdOrNewValue<T>>(
    actionConfiguration: StoreAction<T, U, M>
        ) => StoreBuilder<U> = (action) => {
            this.ensureAllPropsHaveValue(action);
            this.actions.push(action);
            return this;
        }


        /**
         * Creates an observable that wraps the store, dispatcher, and action roles of the flux pattern into one stream.
         * The `DataSource` is used to initialized the value 
         * in the store.  Any provided `StoreAction` objects are used to create streams that mutate the data in the store.
         * 
         * The resulting observable emits the new value to everything subscribed 
         * to it whenever there is a change to the store.  The store keeps the last item emitted.
         * 
         * When an observer subscribes to the observable and the observable has already emitted, the observable 
         * emits the current value.  When the observable has not emitted a value yet, the store is initialized with the provided 
         * `DataSource` @see setDataSource .  Then, the resulting value from the initialized store is passed on to the observer.
         * 
         * When the component needs to restructure the 
         * resulting data (via a selector), the component should attach it's own implementation of the selector 
         * to the resulting observable.  This helps to decouple the store from any view in the app.
         * 
         * When an unhandled exception is thrown the stream stops.
         */
    readonly Build: () => Observable<U> = () => {
        // Push the result of any store updates into the source of the streams we operate on.
        // This is necessary because if we don't have a 'feedback loop' and the store's current value is not a primitive, the streams 
        // might create a different object reference which will cause the streams to be out of sync.
        const feedbackLoop = new ReplaySubject<U>(1);

        const actions$ = this.actions.map(action => this.CreateActionStream(
            feedbackLoop,
            action
        ));

        return this.createStoreStream(this.dataSource, actions$, feedbackLoop).pipe(
            publishReplay(1),
            refCount());
    }

    private createStoreStream(cache$: Observable<U>, actions$: Observable<U>[], feedbackLoop: Subject<U>): Observable<U> {
        const buildStreamWithActions = new Maybe(actions$).map(x => !!x.length).valueOr(false);
        if (buildStreamWithActions) {
            return merge(
                cache$.pipe(first()), // make sure we emit data on first subscription
                merge(
                    ...actions$
                )
            ).pipe(tap(x => feedbackLoop.next(x)));
        } else {
            // no need to set up a complex stream if we aren't going to modify the data.
            return this.dataSource;
        }
    }

          /**
   * Used to convert a `StoreAction` object into an rxjs stream. Returns a stream that represents most if not all of the flux pattern.
   * @param initStore$ An observable that initializes our store
   * @param action The configured action to convert into an rxjs stream
   */
  private CreateActionStream<T, M extends IdOrNewValue<T>>(
    initStore$: Observable<U>,
    action: StoreAction<T, U, M>): Observable<U> {
    return action.dispatcher$.pipe(
      withLatestFrom(initStore$), // we want to continue the stream when we first get the data
      map(x => ({  idOrNewValue: x[0], currentValue: x[1] })),
      action.setupFn,
      action.normalizeSaveHandler((x) => {
        return forkJoin(
            action.effect(x.item, x.idOrNewValue),
          of(x.currentValue));
      },
      action.operation,
      false
      ),
      map(x => action.updateState(x[0], x[1])));
  }
}
