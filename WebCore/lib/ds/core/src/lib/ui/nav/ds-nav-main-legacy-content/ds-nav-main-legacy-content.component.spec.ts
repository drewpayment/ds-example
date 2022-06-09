import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsNavMainLegacyContentComponent } from './ds-nav-main-legacy-content.component';

describe('DsNavMainLegacyContentComponent', () => {
  let component: DsNavMainLegacyContentComponent;
  let fixture: ComponentFixture<DsNavMainLegacyContentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsNavMainLegacyContentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsNavMainLegacyContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
