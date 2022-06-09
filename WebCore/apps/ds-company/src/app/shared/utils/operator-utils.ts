import { RootInjector } from 'lib/utilties/root.injector';
import { MonoTypeOperatorFunction, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MessageService } from '../../services/message.service';

export function catchHttpError<T = any>(
  replacementData?: T,
  customMessage?: string,
  ): MonoTypeOperatorFunction<T> {
  const msg = RootInjector.get(MessageService);
  return catchError(err => {
    console.error(err, 'error');
    msg.setErrorResponse(customMessage || err);
    return of(!!replacementData ? replacementData : null);
  });
}

export function alertAndRethrow<T = any>(
  beforeRethrow?: (error?: any) => void
): MonoTypeOperatorFunction<T> {
  const msg = RootInjector.get(MessageService);
  return catchError(err => {
    console.error(err, 'error');
    msg.setErrorResponse(err);
    beforeRethrow?.(err);
    return throwError(err);
  });
}
