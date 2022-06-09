import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { routes } from './routes';


@NgModule({
    imports: [RouterModule.forRoot(routes, {
        useHash: true,
        // enableTracing: true && !environment.production // ONLY UNCOMMENT TO DEBUG ANGULAR ROUTER IN DEVELOPMENT
    })],
    exports: [RouterModule]
})
export class AppRoutingModule { }
