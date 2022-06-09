import { filter } from 'rxjs/operators';


export function filterNull() {
  return filter(value => value !== undefined && value !== null);
}
