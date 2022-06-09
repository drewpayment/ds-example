import { Maybe } from './Maybe';

export function normalizeString(value: Maybe<string>): Maybe<string> {
    return value
.map(x => x.toLowerCase())
.map(x => x.trim())
}