import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageFinalDisclaimerComponent } from './manage-final-disclaimer.component';

describe('ManageFinalDisclaimerComponent', () => {
  let component: ManageFinalDisclaimerComponent;
  let fixture: ComponentFixture<ManageFinalDisclaimerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageFinalDisclaimerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageFinalDisclaimerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
