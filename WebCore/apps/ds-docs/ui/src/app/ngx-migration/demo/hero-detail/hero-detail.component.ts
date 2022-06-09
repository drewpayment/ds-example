import { Component, Input, Output, EventEmitter } from "@angular/core";
import { Hero } from "../shared/hero";

@Component({
    selector: 'hero-detail',
    templateUrl: './hero-detail.component.html'
})
export class HeroDetailComponent { 
    @Input() hero: Hero;
    @Output() deleted = new EventEmitter<Hero>();
    onDelete() {
        this.deleted.emit(this.hero);
    }
}