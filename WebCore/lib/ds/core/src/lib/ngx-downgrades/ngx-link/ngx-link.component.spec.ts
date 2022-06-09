import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NgxLinkComponent } from './ngx-link.component';

describe('NgxLinkComponent', () => {
  let component: NgxLinkComponent;
  let fixture: ComponentFixture<NgxLinkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NgxLinkComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NgxLinkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
