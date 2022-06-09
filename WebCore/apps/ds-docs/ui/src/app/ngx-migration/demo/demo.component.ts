
import { Hero } from "./shared/hero";

export class NgxDemoRootComponent {

    static SELECTOR = "demo";
    static CONFIG: ng.IComponentOptions = {
        controller: NgxDemoRootComponent,
        template: `
            <hero-detail [hero]="$ctrl.hero" (deleted)="$ctrl.heroDeleted($event)"></hero-detail>
        `
    };

    hero = new Hero(1, "Superman");

    constructor(){
    }

    heroDeleted($event: Hero){
        console.dir($event);
        alert(`${$event.name} deleted | ID ${$event.id}`);
    }
}