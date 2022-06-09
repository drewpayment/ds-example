import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PunchMapModalComponent } from './punch-map-modal.component';

describe('PunchMapModalComponent', () => {
  let component: PunchMapModalComponent;
  let fixture: ComponentFixture<PunchMapModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PunchMapModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PunchMapModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
