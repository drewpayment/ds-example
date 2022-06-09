import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableViewStickyComponent } from './table-view-sticky.component';

describe('TableViewStickyComponent', () => {
  let component: TableViewStickyComponent;
  let fixture: ComponentFixture<TableViewStickyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TableViewStickyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TableViewStickyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
