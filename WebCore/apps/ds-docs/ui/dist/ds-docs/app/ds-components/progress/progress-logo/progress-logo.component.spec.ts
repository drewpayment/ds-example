import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgressLogoComponent } from './progress-logo.component';

describe('ProgressLogoComponent', () => {
  let component: ProgressLogoComponent;
  let fixture: ComponentFixture<ProgressLogoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgressLogoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgressLogoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
