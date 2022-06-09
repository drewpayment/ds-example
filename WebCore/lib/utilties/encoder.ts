// export function encodeQueryParam(secret: string, value: string): string {
//     const salt = `${secret}:${value}`;
//     return encodeURI(btoa(salt));
// }
// export function decodeQueryParam(secret: string, value: string): string {
//     const raw = decodeURI(atob(value));
//     const strArr = raw.split(':');
//     if (strArr[0] !== secret) return null;
//     return strArr[1];
// }

export function encodeQueryParam(queryParam: string): string {
    return encodeURIComponent(btoa(queryParam));
}
export function decodeQueryParam(value: string): string {
    let uriDecoded = decodeURIComponent(value);
    let raw = atob(uriDecoded);
    return raw;
}