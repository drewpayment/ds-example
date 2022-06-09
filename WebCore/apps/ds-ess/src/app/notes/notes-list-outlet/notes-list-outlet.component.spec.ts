import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotesListOutletComponent } from './notes-list-outlet.component';

describe('NotesListOutletComponent', () => {
  let component: NotesListOutletComponent;
  let fixture: ComponentFixture<NotesListOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NotesListOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotesListOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
