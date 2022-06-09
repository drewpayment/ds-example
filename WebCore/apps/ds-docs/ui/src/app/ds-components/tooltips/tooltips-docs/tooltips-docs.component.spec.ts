import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TooltipsDocsComponent } from './tooltips-docs.component';

describe('TooltipsDocsComponent', () => {
  let component: TooltipsDocsComponent;
  let fixture: ComponentFixture<TooltipsDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TooltipsDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TooltipsDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
