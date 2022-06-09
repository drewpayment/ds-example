import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ToggleBtnsLegacyMultipleComponent } from './toggle-btns-legacy-multiple.component';

describe('ToggleBtnsLegacyMultipleComponent', () => {
  let component: ToggleBtnsLegacyMultipleComponent;
  let fixture: ComponentFixture<ToggleBtnsLegacyMultipleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ToggleBtnsLegacyMultipleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ToggleBtnsLegacyMultipleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
