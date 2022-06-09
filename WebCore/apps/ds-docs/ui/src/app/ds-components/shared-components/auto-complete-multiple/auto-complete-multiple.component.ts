import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { BILLMURRAYMOVIES } from '@ds/docs/ds-components/shared/billMurray';
import { IMovies } from '@ds/docs/ds-components/shared/models';

@Component({
  selector: 'ds-auto-complete-multiple',
  templateUrl: './auto-complete-multiple.component.html',
  styleUrls: ['./auto-complete-multiple.component.css']
})
export class AutoCompleteMultipleComponent implements OnInit {
  movieName = new FormControl;
  form: FormGroup;
  movies: IMovies[] = [];
  selectedMovies: string[] = [];
  allMovies: IMovies[] = BILLMURRAYMOVIES;
  filteredMovies: Observable<IMovies[]>;
  @ViewChild('input', {static: false}) input: ElementRef<HTMLInputElement>;
  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.form = this.fb.group({
      movieName: '',
    });
    this.filterMovies();
  }

  displayFn(billMurrayMovie: IMovies): string {
    return billMurrayMovie && billMurrayMovie.movie ? billMurrayMovie.movie : '';
  }

  filterMovies() {
    this.filteredMovies = this.movieName.valueChanges
      .pipe(
        startWith(''),
        map(value => typeof value === 'string' ? value : ''),
        map(movie => movie ? this._filter(movie) : this.allMovies.slice())
      );
  }

  private _filter(m: string): IMovies[] {
    const filterValue = m.toLowerCase();

    return this.movies.filter(m => m.movie.toLowerCase().includes(filterValue));
  }
  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();

    // Add our fruit
    if (value) {
      this.selectedMovies.push(value);
    }

    // Clear the input value
    //event.chipInput.clear();

    this.movieName.setValue(null);
  }

  remove(movie: string): void {
    const index = this.selectedMovies.indexOf(movie);

    if (index >= 0) {
      this.selectedMovies.splice(index, 1);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.selectedMovies.push(event.option.viewValue);
    this.input.nativeElement.value = '';
    this.movieName.setValue(null);
    this.filterMovies();
  }
  selectOption(e: Event, trigger: MatAutocompleteTrigger) {
    e.stopPropagation();
    trigger.openPanel();
  }
}
