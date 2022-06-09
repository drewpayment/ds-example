import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SwitchTitleComponent } from './switch-title.component';

describe('SwitchTitleComponent', () => {
  let component: SwitchTitleComponent;
  let fixture: ComponentFixture<SwitchTitleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SwitchTitleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SwitchTitleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
