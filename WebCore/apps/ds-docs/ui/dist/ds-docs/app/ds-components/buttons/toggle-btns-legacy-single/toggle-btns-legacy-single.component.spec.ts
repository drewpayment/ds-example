import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ToggleBtnsLegacySingleComponent } from './toggle-btns-legacy-single.component';

describe('ToggleBtnsLegacySingleComponent', () => {
  let component: ToggleBtnsLegacySingleComponent;
  let fixture: ComponentFixture<ToggleBtnsLegacySingleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ToggleBtnsLegacySingleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ToggleBtnsLegacySingleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
