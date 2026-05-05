<template>
  <div class="page">
    <div class="page-header">
      <h2>Dashboard</h2>
    </div>

    <!-- Quick actions + recent games -->
    <div class="bottom-grid">
      <div class="card">
        <div class="card-title">Rychlé akce</div>
        <div class="quick-actions">
          <RouterLink :to="{ name: 'games', query: { new: '1' } }" class="quick-action">
            <div class="qa-icon" style="background: var(--green-light)">
              <i class="pi pi-plus" style="color: var(--green-dark)"></i>
            </div>
            Přidat zápas
          </RouterLink>
          <RouterLink to="/clanky/novy" class="quick-action quick-action-blue">
            <div class="qa-icon" style="background: #eff6ff">
              <i class="pi pi-file-edit" style="color: #1d4ed8"></i>
            </div>
            Nový článek
          </RouterLink>
        </div>
      </div>
      
    </div>
  </div>
</template>

<script setup lang="ts">

</script>

<style scoped>
.bottom-grid {
  display: grid;
  grid-template-columns: 1fr 1.6fr;
  gap: 16px;
}

.card {
  background: #fff;
  border-radius: 12px;
  border: 1.5px solid var(--border);
  padding: 20px 22px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, .04);
}

.card-title {
  font-size: 0.8rem;
  font-weight: 700;
  color: #374151;
  text-transform: uppercase;
  letter-spacing: 0.06em;
  margin-bottom: 14px;
}

.quick-actions {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.quick-action {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 12px 14px;
  border-radius: 9px;
  border: 1.5px solid var(--border);
  background: #fafbfc;
  cursor: pointer;
  font-family: inherit;
  font-size: 0.845rem;
  font-weight: 500;
  color: #374151;
  transition: all 0.12s;
  text-decoration: none;
}

.quick-action:hover {
  background: var(--green-light);
  border-color: var(--green);
  color: #374151;
}

.quick-action-blue:hover {
  background: #eff6ff;
  border-color: #93c5fd;
}

.qa-icon {
  width: 30px;
  height: 30px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.8rem;
  flex-shrink: 0;
}

.recent-games {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.games-loading,
.games-empty {
  color: var(--text-muted);
  font-size: 0.845rem;
  padding: 8px 0;
}

.game-row {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 10px 0;
  border-bottom: 1px solid #f3f4f6;
}

.game-row.last {
  border-bottom: none;
}

.game-date {
  width: 40px;
  text-align: center;
  font-size: 0.69rem;
  font-weight: 700;
  color: var(--green-dark);
  background: var(--green-light);
  border-radius: 7px;
  padding: 4px 0;
  flex-shrink: 0;
}

.game-info {
  flex: 1;
}

.game-teams {
  font-size: 0.845rem;
  font-weight: 500;
  color: #111318;
}

.vs {
  color: var(--text-muted);
  font-weight: 400;
  margin: 0 4px;
}

.winner-text {
  font-weight: 700;
  color: #166534;
}

.game-meta {
  font-size: 0.72rem;
  color: var(--text-muted);
}

.game-score {
  font-family: 'DM Mono', 'Menlo', monospace;
  font-weight: 600;
  font-size: 0.845rem;
  color: #111318;
}

.game-score-empty {
  color: var(--text-muted);
}
</style>
