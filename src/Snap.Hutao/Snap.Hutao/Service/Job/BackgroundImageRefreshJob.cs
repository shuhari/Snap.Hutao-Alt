using Quartz;
using Snap.Hutao.Service.BackgroundImage;
using Snap.Hutao.ViewModel;

namespace Snap.Hutao.Service.Job;

internal sealed partial class BackgroundImageRefreshJob : IJob
{
    public BackgroundImageRefreshJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private readonly IServiceProvider _serviceProvider;

    private static DateTime? _lastUpdateTime = null;

    private readonly TimeSpan updateInterval = TimeSpan.FromHours(1);

    [SuppressMessage("", "SH003")]
    public async Task Execute(IJobExecutionContext context)
    {
        // Debug.WriteLine("BackgroundWallpaperRefreshJob.execute");
        var viewModel = _serviceProvider.GetService<MainViewModel>();
        // 如果是每日一图类型的图源，则每隔一定时间自动触发刷新
        if (viewModel != null && IsDailyType(viewModel.AppOptions.BackgroundImageType) && NeedUpdate())
        {
            await viewModel.UpdateBackgroundCommand.ExecuteAsync(false);
            _lastUpdateTime = DateTime.Now;
        }
    }

    private bool IsDailyType(BackgroundImageType imageType)
    {
        return imageType == BackgroundImageType.HutaoDaily || imageType == BackgroundImageType.HutaoBing;
    }

    private bool NeedUpdate()
    {
        if (_lastUpdateTime == null)
            return true;
        TimeSpan interval = TimeSpan.FromHours(4);
        TimeSpan elapsed = DateTime.Now - _lastUpdateTime.Value;
        return elapsed >= interval;
    }
}
