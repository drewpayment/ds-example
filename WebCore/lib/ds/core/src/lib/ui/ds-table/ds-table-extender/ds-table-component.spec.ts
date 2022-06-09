import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsTableExtenderComponent } from './ds-table-extender.component';

describe('DsTableExtenderComponent', () => {
  let component: DsTableExtenderComponent;
  let fixture: ComponentFixture<DsTableExtenderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsTableExtenderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsTableExtenderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
