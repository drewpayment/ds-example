import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsExpansionComponent } from './ds-expansion.component';

describe('DsExpansionComponent', () => {
  let component: DsExpansionComponent;
  let fixture: ComponentFixture<DsExpansionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsExpansionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsExpansionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
