import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsTableComponent } from './ds-table.component';

describe('DsTableComponent', () => {
  let component: DsTableComponent;
  let fixture: ComponentFixture<DsTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
