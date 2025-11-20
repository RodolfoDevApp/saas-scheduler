# Contrato por Bounded Context - Tablas, Endpoints y Eventos (Version 1)

Este documento resume, para cada bounded context del sistema SaaS de gestion de servicios, sus tablas principales, endpoints HTTP sugeridos y eventos de integracion (salida y entrada). Sirve como contrato para implementar un monolito modular o un conjunto de microservicios coordinados por eventos.

## 1. Identity & Tenancy

**Responsabilidad:** multi-tenant (talleres/negocios), usuarios internos, autenticacion y roles base.

### Tablas principales

- Tenants: Id, Name, Slug, Active, CreatedAt.
- Users: Id, TenantId, Name, Email, Phone, PasswordHash, IsActive, CreatedAt.
- Roles: Id, Name (Admin, Supervisor, Operativo, etc.).
- UserRoles: UserId, RoleId.

### Endpoints HTTP (prefijo sugerido: /api/identity)

- POST /api/identity/auth/login
- POST /api/identity/auth/refresh-token
- POST /api/identity/tenants
- GET /api/identity/tenants/{id}
- GET /api/identity/users
- GET /api/identity/users/{id}
- POST /api/identity/users
- PUT /api/identity/users/{id}
- DELETE /api/identity/users/{id}
- GET /api/identity/roles
- POST /api/identity/users/{id}/roles
- DELETE /api/identity/users/{id}/roles/{roleId}

### Eventos de integracion (Out / In)

- Out: TenantRegistered (TenantId, Name, CreatedAt).
- Out: UserRegistered (UserId, TenantId, Name, Email, Roles).
- Out: UserRoleChanged (UserId, TenantId, Roles).
- Out: UserDeactivated (UserId, TenantId).
- In: ninguno obligatorio en v1 (recibe comandos via HTTP).

## 2. Customers & Assets

**Responsabilidad:** clientes finales y activos asociados (carros, maquinas, dispositivos, etc.).

### Tablas principales

- Customers: Id, TenantId, Name, Email, Phone, Notes, CreatedAt, IsActive.
- Assets: Id, TenantId, CustomerId, Type, Identifier, Brand, Model, Year, Color, Notes, CreatedAt, IsActive.

### Endpoints HTTP (/api/customers, /api/assets)

- GET /api/customers
- GET /api/customers/{id}
- POST /api/customers
- PUT /api/customers/{id}
- DELETE /api/customers/{id}
- GET /api/assets
- GET /api/assets/{id}
- GET /api/customers/{customerId}/assets
- POST /api/assets
- PUT /api/assets/{id}
- DELETE /api/assets/{id}

### Eventos de integracion (Out / In)

- Out: CustomerRegistered (CustomerId, TenantId, Name, Email, Phone, CreatedAt).
- Out: CustomerUpdated (CustomerId, TenantId, campos modificados).
- Out: AssetRegistered (AssetId, TenantId, CustomerId, Type, Identifier, Brand, Model).
- Out: AssetUpdated (AssetId, TenantId, campos modificados).
- In: TenantRegistered (opcional, para crear configuraciones por defecto).

## 3. Ticketing & Workflow

**Responsabilidad:** WorkUnits (carro/proyecto/dia), Tickets, Actividades visibles para el cliente y Mini-tareas internas, promesas de espera e historiales de estados.

### Tablas principales

- WorkUnits: Id, TenantId, CustomerId, AssetId, Type, Name, Description, Status, CreatedAt, ClosedAt.
- Tickets: Id, TenantId, WorkUnitId, Code, Title, Description, Status, IsOverdue, TargetDate, CreatedAt, StartedAt, ClosedAt.
- Activities: Id, TenantId, TicketId, Name, Description, Status, EstimatedMinutes, CreatedAt, CompletedAt.
- MiniTasks: Id, TenantId, ActivityId, Title, Description, Status, AssignedEmployeeId, EstimatedMinutes, StartedAt, FinishedAt, CreatedAt.
- WaitPromises: Id, TenantId, AppliesToType, ActivityId, MiniTaskId, Reason, BlockerType, BlockerName, PromiseDate, ReleasedAt, Status, CreatedAt.
- TicketStatusHistory: Id, TenantId, TicketId, OldStatus, NewStatus, ChangedByUserId, ChangedAt, Note.
- ActivityStatusHistory: Id, TenantId, ActivityId, OldStatus, NewStatus, ChangedByUserId, ChangedAt, Note.
- MiniTaskStatusHistory: Id, TenantId, MiniTaskId, OldStatus, NewStatus, ChangedByUserId, ChangedAt, Note.

### Endpoints HTTP (/api/ticketing)

- GET /api/ticketing/work-units
- GET /api/ticketing/work-units/{id}
- POST /api/ticketing/work-units
- PUT /api/ticketing/work-units/{id}
- POST /api/ticketing/work-units/{id}/close
- GET /api/ticketing/tickets
- GET /api/ticketing/tickets/{id}
- POST /api/ticketing/tickets
- PUT /api/ticketing/tickets/{id}
- POST /api/ticketing/tickets/{id}/start
- POST /api/ticketing/tickets/{id}/close
- POST /api/ticketing/tickets/{id}/change-status
- GET /api/ticketing/tickets/{ticketId}/activities
- GET /api/ticketing/activities/{id}
- POST /api/ticketing/tickets/{ticketId}/activities
- PUT /api/ticketing/activities/{id}
- POST /api/ticketing/activities/{id}/change-status
- POST /api/ticketing/activities/{id}/set-estimated-time
- GET /api/ticketing/activities/{activityId}/mini-tasks
- GET /api/ticketing/mini-tasks/{id}
- POST /api/ticketing/activities/{activityId}/mini-tasks
- PUT /api/ticketing/mini-tasks/{id}
- POST /api/ticketing/mini-tasks/{id}/change-status
- POST /api/ticketing/mini-tasks/{id}/assign-employee
- GET /api/ticketing/wait-promises
- POST /api/ticketing/activities/{activityId}/wait-promises
- POST /api/ticketing/mini-tasks/{miniTaskId}/wait-promises
- POST /api/ticketing/wait-promises/{id}/resolve

### Eventos de integracion (Out / In)

- Out: WorkUnitCreated.
- Out: TicketCreated.
- Out: TicketStatusChanged.
- Out: TicketClosed.
- Out: ActivityCreated.
- Out: ActivityStatusChanged.
- Out: MiniTaskCreated (opcional).
- Out: MiniTaskStatusChanged.
- Out: WaitPromiseCreated.
- Out: WaitPromiseFulfilled.
- Out: WaitPromiseBroken.
- Out: TicketDueDateBreached.
- Out: ActivityEstimatedValueSet.
- Out: ActivityClosedWithValue.
- In: AppointmentScheduled (de Scheduling) para crear WorkUnit/Ticket.
- In: AppointmentCancelled / AppointmentRescheduled (opcional).

## 4. Scheduling

**Responsabilidad:** citas en agenda y relacion con tickets y unidades de trabajo.

### Tablas principales

- Appointments: Id, TenantId, CustomerId, AssetId, WorkUnitId, TicketId, ScheduledStart, ScheduledEnd, Status, AssignedEmployeeId, Notes, CreatedAt.

### Endpoints HTTP (/api/scheduling)

- GET /api/scheduling/appointments
- GET /api/scheduling/appointments/{id}
- POST /api/scheduling/appointments
- PUT /api/scheduling/appointments/{id}
- POST /api/scheduling/appointments/{id}/cancel
- POST /api/scheduling/appointments/{id}/reschedule
- POST /api/scheduling/appointments/{id}/link-ticket

### Eventos de integracion (Out / In)

- Out: AppointmentScheduled.
- Out: AppointmentRescheduled.
- Out: AppointmentCancelled.
- Out: TicketLinkedToAppointment.
- In: TicketCreated (para autovincular tickets a citas).
- In: TicketClosed (para marcar cita como Completada si aplica).

## 5. Workforce

**Responsabilidad:** empleados operativos, equipos de trabajo y lideres. Define quien puede crear tareas y quien puede cancelar o marcar incompletas actividades/mini-tareas.

### Tablas principales

- Employees: Id, TenantId, UserId, Name, IsActive, CreatedAt.
- Teams: Id, TenantId, Name, CreatedAt.
- TeamMembers: TeamId, EmployeeId, IsLeader.

### Endpoints HTTP (/api/workforce)

- GET /api/workforce/employees
- GET /api/workforce/employees/{id}
- POST /api/workforce/employees
- PUT /api/workforce/employees/{id}
- POST /api/workforce/employees/{id}/activate
- POST /api/workforce/employees/{id}/deactivate
- GET /api/workforce/teams
- GET /api/workforce/teams/{id}
- POST /api/workforce/teams
- PUT /api/workforce/teams/{id}
- POST /api/workforce/teams/{teamId}/members
- DELETE /api/workforce/teams/{teamId}/members/{employeeId}
- POST /api/workforce/teams/{teamId}/members/{employeeId}/set-leader

### Eventos de integracion (Out / In)

- Out: EmployeeCreated.
- Out: EmployeeUpdated.
- Out: TeamCreated.
- Out: EmployeeAssignedToTeam.
- Out: LeaderChanged.
- In: UserRegistered (de Identity).
- In: UserDeactivated.

## 6. Billing Lite

**Responsabilidad:** montos por actividad, totales por ticket y unidad de trabajo, generacion de nota de venta del taller (no del SaaS).

### Tablas principales

- ActivityAmounts: ActivityId, TenantId, TicketId, WorkUnitId, EstimatedAmount, FinalAmount, Currency, UpdatedAt.
- SaleNotes: Id, TenantId, CustomerId, WorkUnitId, TicketId, TotalAmount, Currency, PaymentStatus, PaidAt, PaymentMethod, ExternalReference, CreatedAt.
- SaleNoteLines: Id, SaleNoteId, ActivityId, Description, Amount.

### Endpoints HTTP (/api/billing)

- GET /api/billing/activities/{activityId}/amount
- POST /api/billing/activities/{activityId}/amount
- GET /api/billing/sale-notes
- GET /api/billing/sale-notes/{id}
- POST /api/billing/tickets/{ticketId}/sale-notes
- POST /api/billing/sale-notes/{id}/mark-paid

### Eventos de integracion (Out / In)

- Out: ActivityAmountUpdated.
- Out: SaleNoteGenerated.
- Out: SaleNotePaymentStatusUpdated.
- In: ActivityEstimatedValueSet (de Ticketing).
- In: ActivityClosedWithValue (de Ticketing, opcional).
- In: TicketClosed.

## 7. Reporting & Analytics

**Responsabilidad:** proyecciones de lectura (read models) y KPIs para duenos, lideres y operadores.

### Tablas de proyecciones (ejemplos)

- TicketMetrics: Id, TenantId, TicketId, Status, LeadTimeMinutes, CycleTimeMinutes, CreatedAt, ClosedAt.
- ActivityMetrics: Id, TenantId, ActivityId, TicketId, Status, EstimatedMinutes, RealMinutes, EstimatedAmount, FinalAmount.
- EmployeeMetrics: Id, TenantId, EmployeeId, Period, TasksCompleted, ActivitiesCompleted, HoursWorked.
- RevenueByWorkUnit: Id, TenantId, WorkUnitId, TotalAmount, Period.

### Endpoints HTTP (/api/reporting)

- GET /api/reporting/overview
- GET /api/reporting/tickets
- GET /api/reporting/employees
- GET /api/reporting/revenue

### Eventos de integracion (Out / In)

- In: CustomerRegistered, AssetRegistered.
- In: TicketCreated, TicketStatusChanged, TicketClosed, TicketDueDateBreached.
- In: ActivityCreated, ActivityStatusChanged.
- In: MiniTaskStatusChanged.
- In: WaitPromiseCreated, WaitPromiseFulfilled, WaitPromiseBroken.
- In: AppointmentScheduled, AppointmentRescheduled, AppointmentCancelled.
- In: ActivityAmountUpdated, SaleNoteGenerated, SaleNotePaymentStatusUpdated.
- In: EmployeeAssignedToTeam, LeaderChanged.
- In: SubscriptionStatusChanged, InvoiceGenerated, InvoicePaid (para reportes del SaaS).

## 8. Realtime & Notifications

**Responsabilidad:** consumir eventos de integracion y empujar cambios en tiempo real a clientes internos y externos via WebSockets/SignalR.

### Tablas opcionales

- NotificationSettings: Id, TenantId, UserId, Channel, Enabled.
- NotificationsLog: Id, TenantId, UserId, Type, PayloadJson, CreatedAt.

### Endpoints / Hubs

- WS/SignalR Hub: /hubs/realtime (grupos tenant:{TenantId}, user:{UserId}, workunit:{WorkUnitId}, ticket:{TicketId}).
- GET /api/realtime/notifications (historial, opcional).
- POST /api/realtime/test (endpoint para pruebas, opcional).

### Eventos de integracion (Out / In)

- In: TicketCreated, TicketStatusChanged, TicketClosed.
- In: ActivityCreated, ActivityStatusChanged.
- In: MiniTaskStatusChanged.
- In: WaitPromiseCreated, WaitPromiseBroken.
- In: AppointmentScheduled, AppointmentRescheduled, AppointmentCancelled.
- In: SaleNoteGenerated, SaleNotePaymentStatusUpdated.
- In: SubscriptionStatusChanged.
- Out: mensajes en tiempo real via WebSocket (por ejemplo: ticketUpdated, activityStatusChanged, miniTaskStatusChanged, waitPromiseBroken, appointmentUpdated, saleNoteGenerated, subscriptionStatusChanged).

## 9. SaaS Billing & Subscriptions

**Responsabilidad:** relacion comercial entre el proveedor del SaaS y los talleres/negocios (tenants). Maneja planes, suscripciones, facturas e informacion de pagos del SaaS, separado del billing interno del taller.

### Tablas principales

- Plans: Id, Name, Description, PriceMonthly, Currency, TrialDays, FeaturesJson, Active.
- Subscriptions: Id, TenantId, PlanId, Status (Trial, Active, PastDue, Cancelled), StartedAt, CurrentPeriodStart, CurrentPeriodEnd, CancelledAt.
- Invoices: Id, SubscriptionId, TenantId, PeriodStart, PeriodEnd, Amount, Currency, Status (Pending, Paid, Failed), DueDate, IssuedAt, PaidAt.
- Payments: Id, InvoiceId, Amount, Currency, Status (Success, Failed), Provider, ProviderReference, PaidAt.
- UsageRecords (opcional): Id, SubscriptionId, TenantId, Period, MetricName, MetricValue.

### Endpoints HTTP (/api/saas)

#### Planes

- GET /api/saas/plans
- GET /api/saas/plans/{id}
- POST /api/saas/plans
- PUT /api/saas/plans/{id}
- POST /api/saas/plans/{id}/activate
- POST /api/saas/plans/{id}/deactivate

#### Suscripciones

- GET /api/saas/subscriptions?tenantId={tenantId}
- GET /api/saas/subscriptions/{id}
- POST /api/saas/subscriptions
- POST /api/saas/subscriptions/{id}/cancel
- POST /api/saas/subscriptions/{id}/resume

#### Facturas (Invoices)

- GET /api/saas/tenants/{tenantId}/invoices
- GET /api/saas/invoices/{id}
- POST /api/saas/subscriptions/{subscriptionId}/invoices/generate-manual

#### Pagos (Payments)

- GET /api/saas/invoices/{invoiceId}/payments
- GET /api/saas/payments/{id}
- POST /api/saas/payments/manual

#### Webhooks de proveedor de pagos (futuro)

- POST /api/saas/webhooks/payment-provider

### Eventos de integracion (Out / In)

**Eventos Out**

- SubscriptionCreated: SubscriptionId, TenantId, PlanId, Status, StartedAt, CurrentPeriodStart, CurrentPeriodEnd.
- SubscriptionStatusChanged: SubscriptionId, TenantId, OldStatus, NewStatus, ChangedAt.
- InvoiceGenerated: InvoiceId, SubscriptionId, TenantId, PeriodStart, PeriodEnd, Amount, Currency, DueDate, IssuedAt.
- InvoicePaid: InvoiceId, SubscriptionId, TenantId, PaidAt, Amount, Currency, PaymentId.
- InvoicePaymentFailed: InvoiceId, SubscriptionId, TenantId, FailedAt, Reason.
- PaymentRecorded: PaymentId, InvoiceId, SubscriptionId, TenantId, Amount, Currency, Status, Provider.

**Eventos In**

- TenantRegistered (de Identity & Tenancy): crear suscripcion en Trial.
- TenantDeactivated (de Identity & Tenancy): cancelar o pausar suscripcion.
- Opcional: UsageEvents de otros BC para planes basados en consumo.

## 10. Nota final

Este documento consolida la vista de responsabilidades, modelo de datos base, endpoints y eventos de integracion para todos los bounded contexts: Identity & Tenancy, Customers & Assets, Ticketing & Workflow, Scheduling, Workforce, Billing Lite, Reporting & Analytics, Realtime & Notifications y SaaS Billing & Subscriptions. Puede usarse como referencia unica para mantener consistencia al pasar de diseno a implementacion.
