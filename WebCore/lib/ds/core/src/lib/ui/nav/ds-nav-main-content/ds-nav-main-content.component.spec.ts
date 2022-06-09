import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsNavMainContentComponent } from './ds-nav-main-content.component';

describe('DsNavMainContentComponent', () => {
  let component: DsNavMainContentComponent;
  let fixture: ComponentFixture<DsNavMainContentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsNavMainContentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsNavMainContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
