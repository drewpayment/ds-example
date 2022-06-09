import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { Observable } from 'rxjs/internal/Observable';
import { map, startWith } from 'rxjs/operators';
import { BILLMURRAYMOVIES } from '../../shared/billMurray';
import { IMovies } from '../../shared/models';

@Component({
  selector: 'ds-auto-complete',
  templateUrl: './auto-complete.component.html',
  styleUrls: ['./auto-complete.component.css']
})
export class AutoCompleteComponent implements OnInit {
  movieName = new FormControl;
  form: FormGroup;
  movies: IMovies[] = BILLMURRAYMOVIES;
  filteredMovies: Observable<IMovies[]>;
  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.form = this.fb.group({
      movieName: '',
    });

    this.filteredMovies = this.movieName.valueChanges
      .pipe(
        startWith(''),
        map(value => typeof value === 'string' ? value : value.name),
        map(movie => movie ? this._filter(movie) : this.movies.slice())
      );
  }

  displayFn(billMurrayMovie: IMovies): string {
    return billMurrayMovie && billMurrayMovie.movie ? billMurrayMovie.movie : '';
  }

  private _filter(m: string): IMovies[] {
    const filterValue = m.toLowerCase();

    return this.movies.filter(m => m.movie.toLowerCase().includes(filterValue));
  }

}
