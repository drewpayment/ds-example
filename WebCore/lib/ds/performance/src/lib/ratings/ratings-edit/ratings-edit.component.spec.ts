import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RatingsEditComponent } from './ratings-edit.component';

describe('RatingsEditComponent', () => {
  let component: RatingsEditComponent;
  let fixture: ComponentFixture<RatingsEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RatingsEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RatingsEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
