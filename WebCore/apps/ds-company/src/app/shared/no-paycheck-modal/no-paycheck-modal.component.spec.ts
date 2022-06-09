import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NoPaycheckModalComponent } from './no-paycheck-modal.component';

describe('NoPaycheckModalComponent', () => {
  let component: NoPaycheckModalComponent;
  let fixture: ComponentFixture<NoPaycheckModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NoPaycheckModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NoPaycheckModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
