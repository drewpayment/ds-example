import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsNavToolbarContentComponent } from './ds-nav-toolbar-content.component';

describe('DsNavToolbarContentComponent', () => {
  let component: DsNavToolbarContentComponent;
  let fixture: ComponentFixture<DsNavToolbarContentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsNavToolbarContentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsNavToolbarContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
