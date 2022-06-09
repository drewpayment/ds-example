// interface Window {
//     [key:string]:any;
// }

interface JQuery {
    [key:string]:any
     cropper(action: string, options?: any) : any;
     getCroppedCanvas(options?: object) : any;
     cropit(...args: any[]) : any;
     mask(...args:any[]):JQuery;
 }

declare var _:any;