import { WrappedValue } from '@angular/core';

type Nil = undefined | null;
const isNil = (x: any): x is Nil =>
  x === null || x === undefined;

  /**
   * Not sure this has enough stuff in it to actually call it a maybe 'monad' but it has enough for it to be useful.
   * Code from: https://spin.atomicobject.com/2018/09/03/maybe-monad-reducing-null-errors/
   */
export class Maybe<T> implements WrappedValue {
  private wrappedValue: T | Nil;

  constructor(value: T | Nil) {
    this.wrappedValue = value;
  }

  get wrapped() { return this.value(); }
  set wrapped(value: any) {
    this.wrappedValue = value;
  }

  public map<U>(fn: ((x: T) => U)): Maybe<U> {
    if (!isNil(this.wrappedValue)) {
      return new Maybe(fn(this.wrappedValue));
    } else {
      return new Maybe<U>(undefined);
    }
  }

  public value(): T | undefined {
    return this.wrappedValue;
  }

  public valueOr<U>(backupValue: U): T | U {
    if (!isNil(this.wrappedValue)) {
      return this.wrappedValue;
    } else {
      return backupValue;
    }
  }

  public ap<U>(val: Maybe<(something: T) => U>): Maybe<U> {
    if (!isNil(this.wrappedValue)) {
      return val.map(x => x(this.wrappedValue));
    } else {
      return new Maybe<U>(undefined);
    }
  }

  public flatMap<U>(fn: ((x: T) => Maybe<U>)): Maybe<U> {
    if (!isNil(this.wrappedValue)) {
      return fn(this.wrappedValue);
    } else {
      return new Maybe<U>(undefined);
    }
  }
}