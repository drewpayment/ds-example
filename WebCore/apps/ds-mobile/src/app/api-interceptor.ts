import { Injectable, Inject } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DOCUMENT } from '@angular/common';
import { AppService } from './app.service';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
    
    private baseApi: string;
    constructor(@Inject('window') private window: any, private service: AppService) {
        if (!this.baseApi) {
            this.baseApi = this.window.apiUrl;
            this.service.updateBaseUrl(this.baseApi);
        }
    }
    
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // const request = req.clone({ url: `${req.url}` });
        const request = req.clone({ url: `${this.baseApi}${req.url}` });
        return next.handle(request);
    }
    
}
