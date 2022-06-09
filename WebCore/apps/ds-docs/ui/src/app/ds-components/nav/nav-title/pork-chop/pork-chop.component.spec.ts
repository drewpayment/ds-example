import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PorkChopComponent } from './pork-chop.component';

describe('PorkChopComponent', () => {
  let component: PorkChopComponent;
  let fixture: ComponentFixture<PorkChopComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PorkChopComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PorkChopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
