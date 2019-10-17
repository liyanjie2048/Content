using System;
using System.Collections.Generic;

namespace Liyanjie.Contents.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentsBuilder
    {
        internal readonly IDictionary<Type, object> Modules = new Dictionary<Type, object>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <typeparam name="TModuleOptions"></typeparam>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public ContentsBuilder AddModule<TModule, TModuleOptions>(Action<TModuleOptions> configureOptions)
            where TModule : class, IContentsModule
            where TModuleOptions : class
        {
            var options = (TModuleOptions)Activator.CreateInstance(typeof(TModuleOptions));
            configureOptions?.Invoke(options);
            Modules.Add(typeof(TModule), options);

            return this;
        }
    }
}
