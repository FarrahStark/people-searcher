import { Observable } from 'rxjs';

export const skipIf = <T>(condition: (value: T) => boolean) => (source: Observable<T>) =>
  new Observable(observer => {
    return source.subscribe({
      next(x: T) {
        if (condition(x)) {
          return;
        }

        observer.next(x);
      },
      error(err) { observer.error(err); },
      complete() { observer.complete(); }
    });
  });
