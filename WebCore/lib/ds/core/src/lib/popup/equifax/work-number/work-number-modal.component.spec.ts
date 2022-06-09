import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkNumberModalComponent } from './work-number-modal.component';

describe('WorkNumberComponent', () => {
  let component: WorkNumberModalComponent;
  let fixture: ComponentFixture<WorkNumberModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkNumberModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkNumberModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
