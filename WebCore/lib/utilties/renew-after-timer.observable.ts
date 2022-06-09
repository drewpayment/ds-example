import { defer, isObservable, Observable, of } from 'rxjs';
import { first, mergeMap, shareReplay } from 'rxjs/operators';


let returnReviewAfterTimerObs$: Observable<any>;
const createRenewAfterTimerObs$ = (obs: Observable<any>, time: number, bufferReplays: number) =>
  bufferReplays < 1 ? (returnReviewAfterTimerObs$ = obs.pipe(shareReplay())) : (returnReviewAfterTimerObs$ = obs.pipe(shareReplay(bufferReplays, time)));

export function renewAfterTimer(obs: Observable<any>, time: number, bufferReplays = 0) {
  return createRenewAfterTimerObs$(obs, time, bufferReplays).pipe(
    first(null, defer(() => createRenewAfterTimerObs$(obs, time, bufferReplays))),
    mergeMap(d => (isObservable(d) ? d : of(d))),
  );
}
