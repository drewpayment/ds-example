import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../environments/environment';

@Injectable()
export class DogfoodInterceptor implements HttpInterceptor {
  private get payrollUrl(): string {
    const url = location.href;
    const uri = new URL(url);
    return uri.pathname.split('/')[1];
  }
  private dogfood = environment.dogfood;
  
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!environment.useDogfood) return next.handle(req);
    
    const newUrl = new URL(req.urlWithParams, location.href);
    const dogfoodPath = this.replacePayrollPath(newUrl);
    const url = `${newUrl.protocol}//${newUrl.host}${dogfoodPath}${newUrl.search}${newUrl.hash}`;
    
    const newReq = req.clone({url});
    return next.handle(newReq);
  }
  
  private replacePayrollPath(url: URL): string {
    const pathParts = url.pathname.split('/');
    
    if (pathParts[1].trim().toLowerCase() === this.payrollUrl) {
      pathParts[1] = this.dogfood;
    } 
    
    return pathParts.join('/');
  }
  
}