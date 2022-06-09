import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ToggleBtnsMatSingleComponent } from './toggle-btns-mat-single.component';

describe('ToggleBtnsMatSingleComponent', () => {
  let component: ToggleBtnsMatSingleComponent;
  let fixture: ComponentFixture<ToggleBtnsMatSingleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ToggleBtnsMatSingleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ToggleBtnsMatSingleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
