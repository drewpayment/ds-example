import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReferenceDateComponent } from './reference-date.component';

describe('ReferenceDateComponent', () => {
  let component: ReferenceDateComponent;
  let fixture: ComponentFixture<ReferenceDateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReferenceDateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReferenceDateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
