import { Pipe, PipeTransform } from '@angular/core';
import { NavHistoryItem } from '@ds/core/shared';

@Pipe({
    name: 'checkActiveState',
})
export class CheckActiveStatePipe implements PipeTransform {
    
    transform(navHistory: NavHistoryItem[], id: number) {
        if (navHistory == null || id == null || id === undefined)
            return false;
            
        if (id === 0) {
            return navHistory.length === 0;
        }
        
        return Array.isArray(navHistory) ? (navHistory.findIndex(nh => nh.dest === id) > -1) : false;
    }
    
}
