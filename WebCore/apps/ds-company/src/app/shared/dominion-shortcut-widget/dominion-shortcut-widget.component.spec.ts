import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DominionShortcutWidgetComponent } from './dominion-shortcut-widget.component';

describe('DominionShortcutWidgetComponent', () => {
  let component: DominionShortcutWidgetComponent;
  let fixture: ComponentFixture<DominionShortcutWidgetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DominionShortcutWidgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DominionShortcutWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
