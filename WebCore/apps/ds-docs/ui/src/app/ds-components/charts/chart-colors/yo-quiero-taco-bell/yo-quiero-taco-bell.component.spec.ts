import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { YoQuieroTacoBellComponent } from './yo-quiero-taco-bell.component';

describe('YoQuieroTacoBellComponent', () => {
  let component: YoQuieroTacoBellComponent;
  let fixture: ComponentFixture<YoQuieroTacoBellComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ YoQuieroTacoBellComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(YoQuieroTacoBellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
