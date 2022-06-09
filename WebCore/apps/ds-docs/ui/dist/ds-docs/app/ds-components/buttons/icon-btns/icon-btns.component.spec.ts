import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IconBtnsComponent } from './icon-btns.component';

describe('IconBtnsComponent', () => {
  let component: IconBtnsComponent;
  let fixture: ComponentFixture<IconBtnsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IconBtnsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IconBtnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
