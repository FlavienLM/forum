
const dateUtils = {
    formatCurrentDate(): string {
        const date = new Date();
        return new Date(date.getTime() - (date.getTimezoneOffset() * 60000)).toISOString();
    },

    timeAgo(dateString: string): string {
        const now = new Date();
        const date = new Date(dateString);
        const diffInSeconds = Math.floor((now.getTime() - date.getTime()) / 1000);
    
        const units = [
            { name: 'an', seconds: 365 * 24 * 60 * 60 },
            { name: 'mois', seconds: 30 * 24 * 60 * 60 },
            { name: 'semaine', seconds: 7 * 24 * 60 * 60 },
            { name: 'jour', seconds: 24 * 60 * 60 },
            { name: 'heure', seconds: 60 * 60 },
            { name: 'minute', seconds: 60 },
            { name: 'seconde', seconds: 1 },
        ];
    
        for (const unit of units) {
            const interval = Math.floor(diffInSeconds / unit.seconds);
            if (interval >= 1) {
                return `Il y a ${interval} ${unit.name}${interval > 1 ? 's' : ''}`;
            }
        }
    
        return "Il y a l'instant";
    }
}

export default dateUtils;

