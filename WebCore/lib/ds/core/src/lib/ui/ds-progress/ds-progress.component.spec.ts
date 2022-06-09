import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsProgressComponent } from './ds-progress.component';

describe('DsProgressComponent', () => {
  let component: DsProgressComponent;
  let fixture: ComponentFixture<DsProgressComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsProgressComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsProgressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
