using Quartz;

namespace Snap.Hutao.Service.Job;

[ConstructorGenerated]
[Injection(InjectAs.Transient, typeof(IJobScheduler))]
internal sealed partial class BackgroundImageRefreshJobScheduler : IJobScheduler
{
    public async ValueTask ScheduleAsync(IScheduler scheduler)
    {
        TimeSpan interval = TimeSpan.FromHours(1);
        TimeSpan delay = TimeSpan.FromSeconds(10);

        IJobDetail job = JobBuilder.Create<BackgroundImageRefreshJob>()
            .WithIdentity(JobIdentity.BackgroundImageJobName, JobIdentity.BackgroundImageGroupName)
            .Build();

        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity(JobIdentity.BackgroundImageTriggerName, JobIdentity.BackgroundImageGroupName)
            .WithSimpleSchedule(builder => builder.WithInterval(interval).RepeatForever())
            .StartAt(DateTimeOffset.Now + delay)
            .Build();

        await scheduler.ScheduleJob(job, trigger).ConfigureAwait(false);
    }
}