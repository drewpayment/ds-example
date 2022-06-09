import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PunchCardComponent } from './punch-card.component';

describe('PunchCardComponent', () => {
  let component: PunchCardComponent;
  let fixture: ComponentFixture<PunchCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PunchCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PunchCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
