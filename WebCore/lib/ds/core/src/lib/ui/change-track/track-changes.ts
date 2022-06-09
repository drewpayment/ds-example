

/**
 * On ASPX pages, vanilla javascript is enabled to listen and track changes 
 * on the page and save them to `window.dominion.tracker`. This function simply
 * listens for the user's attempt to navigate away from the current page and checks 
 * to see the window has change tracking enabled. If it does, it checks to see if 
 * there are any properties with changes existing and then alerts the user about those 
 * unsaved changes existing and that continuing to navigate away will cause a loss of 
 * unsaved work.
 */
export function handleDsChangeTracking() {
    (window as any).addEventListener('beforeunload', (event) => {
        const hasChanges = (window as any).dominion &&
            (window as any).dominion.tracker &&
            (window as any).dominion.tracker.hasChanges != null &&
            (window as any).dominion.tracker.hasChanges();

        if (hasChanges) {
            event.preventDefault();
            event.returnValue = 'Your changes will not be saved.';
        }

        return event;
    });
}
