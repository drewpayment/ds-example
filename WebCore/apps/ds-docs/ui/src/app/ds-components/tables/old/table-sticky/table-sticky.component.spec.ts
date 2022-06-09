import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TableStickyComponent } from './table-sticky.component';

describe('TableStickyComponent', () => {
  let component: TableStickyComponent;
  let fixture: ComponentFixture<TableStickyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TableStickyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TableStickyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
