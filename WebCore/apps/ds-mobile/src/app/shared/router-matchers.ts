import { UrlSegment, UrlMatchResult } from '@angular/router';

export function emergencyContactUpdatePageMatcher(segments: UrlSegment[]): UrlMatchResult {
    if (segments.length > 0 && segments[0].path === 'update') {
        if (segments.length === 1) {
            return {
                consumed: segments,
                posParams: {}
            };
        }
        if (segments.length === 2) {
            return {
                consumed: segments,
                posParams: { id: segments[1] }
            };
        }
    }
    return <UrlMatchResult>(null as any);
}

export function dependentUpdatePageMatcher(segments: UrlSegment[]): UrlMatchResult {
    if (segments.length > 0 && segments[0].path === 'update') {
        if (segments.length === 1) {
            return {
                consumed: segments,
                posParams: {}
            };
        }
        if (segments.length === 2) {
            return {
                consumed: segments,
                posParams: { id: segments[1] }
            };
        }
    }    
    return <UrlMatchResult>(null as any);
}
