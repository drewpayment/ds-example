import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NachoCheeseComponent } from './nacho-cheese.component';

describe('NachoCheeseComponent', () => {
  let component: NachoCheeseComponent;
  let fixture: ComponentFixture<NachoCheeseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NachoCheeseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NachoCheeseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
