import { InjectFlags, InjectionToken, Injector, Type } from '@angular/core';
import { BehaviorSubject } from 'rxjs';


export class RootInjector {
  private static rootInjector: Injector;
  private static readonly $injectorReady = new BehaviorSubject<boolean>(false);
  readonly injectorReady$ = RootInjector.$injectorReady.asObservable();

  static setInjector(injector: Injector) {
    if (this.rootInjector) return;

    this.rootInjector = injector;
    this.$injectorReady.next(true);
  }

  static get<T>(token: Type<T> | InjectionToken<T>, notFoundValue?: T, flags?: InjectFlags): T {
    try {
      return this.rootInjector.get(token, notFoundValue, flags);
    } catch (e) {
      console.error(`
        Error getting ${token} from RootInjector. This is likely due to RootInjector being undefined. Please
        check RootInjector.rootInjector existence.
      `);
      return null;
    }
  }
}
